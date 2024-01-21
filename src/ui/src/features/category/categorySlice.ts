import { createAsyncThunk, createEntityAdapter, createSlice } from "@reduxjs/toolkit";
import agent from "../../app/api/agent";
import { Category, CategoryParams } from "../../app/models/category";
import { RootState } from '../../app/store/configureStore';
import { MetaData } from '../../app/models/pagination';

interface CategoryState {
    categoriesLoaded: boolean;
    status: string;
    categoryParams: CategoryParams;
    metaData: MetaData | null;
}

const categoriesAdapter = createEntityAdapter<Category>();

function getAxiosParams(categoryParams: CategoryParams) {
    const params = new URLSearchParams();
    params.append('pageNumber', categoryParams.pageNumber.toString());
    params.append('pageSize', categoryParams.pageSize.toString());
    return params;
}

export const fetchCategoriesAsync = createAsyncThunk<Category[], void, {state: RootState}>(
    'category/fetchCategoriesAsync',
    async (_, thunkAPI) => {
        const params = getAxiosParams(thunkAPI.getState().category.categoryParams);
        try {
            const response = await agent.Category.list(params);
            thunkAPI.dispatch(setMetaData(response.metaData));
            return response.result;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data })
        }
    }
)

export const fetchCategoryAsync = createAsyncThunk<Category, string>(
    'category/fetchCategoryAsync',
    async (categoryId, thunkAPI) => {
        try {
            const category = await agent.Category.details(categoryId);
            return category;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data })
        }
    }
)

function initParams(): CategoryParams {
    return {
        pageNumber: 1,
        pageSize: 6,
    }
}

export const categorySlice = createSlice({
    name: 'category',
    initialState: categoriesAdapter.getInitialState<CategoryState>({
        categoriesLoaded: false,
        status: 'idle',
        categoryParams: initParams(),
        metaData: null
    }),
    reducers: {
        setCategoryParams: (state, action) => {
            state.categoriesLoaded = false;
            state.categoryParams = {...state.categoryParams, ...action.payload, pageNumber: 1}
        },
        setPageNumber: (state, action) => {
            state.categoriesLoaded = false;
            state.categoryParams = {...state.categoryParams, ...action.payload}
        },
        setMetaData: (state, action) => {
            state.metaData = action.payload
        },
        resetCategoryParams: (state) => {
            state.categoryParams = initParams()
        },
        setCategory: (state, action) => {
            categoriesAdapter.upsertOne(state, action.payload);
            state.categoriesLoaded = false;
        },
        removeCategory: (state, action) => {
            categoriesAdapter.removeOne(state, action.payload);
            state.categoriesLoaded = false;
        }
    },
    extraReducers: (builder => {
        builder.addCase(fetchCategoriesAsync.pending, (state) => {
            state.status = 'pendingFetchCategories'
        });
        builder.addCase(fetchCategoriesAsync.fulfilled, (state, action) => {
            categoriesAdapter.setAll(state, action.payload);
            state.status = 'idle',
                state.categoriesLoaded = true;
        });
        builder.addCase(fetchCategoriesAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });
        builder.addCase(fetchCategoryAsync.pending, (state) => {
            state.status = 'pendingFetchCategory'
        });
        builder.addCase(fetchCategoryAsync.fulfilled, (state, action) => {
            categoriesAdapter.upsertOne(state, action.payload);
            state.status = 'idle'
        });
        builder.addCase(fetchCategoryAsync.rejected, (state, action) => {
            console.log(action);
            state.status = 'idle'
        });        
    })
})

export const {setCategoryParams, resetCategoryParams, setMetaData, setPageNumber, setCategory, removeCategory} = categorySlice.actions;

export const categorySelectors = categoriesAdapter.getSelectors((state: RootState) => state.category);