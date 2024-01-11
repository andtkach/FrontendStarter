import { createAsyncThunk, createEntityAdapter, createSlice } from "@reduxjs/toolkit";
import agent from "../../app/api/agent";
import { Person, PersonParams } from "../../app/models/person";
import { RootState } from '../../app/store/configureStore';
import { MetaData } from '../../app/models/pagination';

interface PersonState {
    personsLoaded: boolean;
    status: string;
    personParams: PersonParams;
    metaData: MetaData | null;
}

const personsAdapter = createEntityAdapter<Person>();

function getAxiosParams(personParams: PersonParams) {
    console.log(personParams);
    const params = new URLSearchParams();
    return params;
}

export const fetchPersonsAsync = createAsyncThunk<Person[], void, {state: RootState}>(
    'person/fetchPersonsAsync',
    async (_, thunkAPI) => {
        const params = getAxiosParams(thunkAPI.getState().person.personParams);
        try {
            const response = await agent.Person.list(params);
            thunkAPI.dispatch(setMetaData(response.metaData));
            return response;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data })
        }
    }
)

export const fetchPersonAsync = createAsyncThunk<Person, string>(
    'person/fetchPersonAsync',
    async (personId, thunkAPI) => {
        try {
            const person = await agent.Person.details(personId);
            return person;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data })
        }
    }
)

function initParams(): PersonParams {
    return {
        pageNumber: 1,
        pageSize: 6,
    }
}

export const personSlice = createSlice({
    name: 'person',
    initialState: personsAdapter.getInitialState<PersonState>({
        personsLoaded: false,
        status: 'idle',
        personParams: initParams(),
        metaData: null
    }),
    reducers: {
        setPersonParams: (state, action) => {
            state.personsLoaded = false;
            state.personParams = {...state.personParams, ...action.payload, pageNumber: 1}
        },
        setPageNumber: (state, action) => {
            state.personsLoaded = false;
            state.personParams = {...state.personParams, ...action.payload}
        },
        setMetaData: (state, action) => {
            state.metaData = action.payload
        },
        resetPersonParams: (state) => {
            state.personParams = initParams()
        },
        setPerson: (state, action) => {
            personsAdapter.upsertOne(state, action.payload);
            state.personsLoaded = false;
        },
        removePerson: (state, action) => {
            personsAdapter.removeOne(state, action.payload);
            state.personsLoaded = false;
        }
    },
    extraReducers: (builder => {
        builder.addCase(fetchPersonsAsync.pending, (state) => {
            state.status = 'pendingFetchPersons'
        });
        builder.addCase(fetchPersonsAsync.fulfilled, (state, action) => {
            personsAdapter.setAll(state, action.payload);
            state.status = 'idle',
                state.personsLoaded = true;
        });
        builder.addCase(fetchPersonsAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });
        builder.addCase(fetchPersonAsync.pending, (state) => {
            state.status = 'pendingFetchPerson'
        });
        builder.addCase(fetchPersonAsync.fulfilled, (state, action) => {
            personsAdapter.upsertOne(state, action.payload);
            state.status = 'idle'
        });
        builder.addCase(fetchPersonAsync.rejected, (state, action) => {
            console.log(action);
            state.status = 'idle'
        });        
    })
})

export const {setPersonParams, resetPersonParams, setMetaData, setPageNumber, setPerson, removePerson} = personSlice.actions;

export const personSelectors = personsAdapter.getSelectors((state: RootState) => state.person);