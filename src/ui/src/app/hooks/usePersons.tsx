import { useEffect } from "react";
import { personSelectors, fetchPersonsAsync } from "../../features/person/personSlice";
import { useAppSelector, useAppDispatch } from "../store/configureStore";

export default function usePersons() {
    const persons = useAppSelector(personSelectors.selectAll);
    const { personsLoaded, metaData} = useAppSelector(state => state.person);
    const dispatch = useAppDispatch();
  
    useEffect(() => {
      if (!personsLoaded) dispatch(fetchPersonsAsync());
    }, [personsLoaded, dispatch])
  
    return {
        persons,
        personsLoaded,
        metaData
    }
}