import { useEffect } from "react";
import { categorySelectors, fetchCategoriesAsync } from "../../features/category/categorySlice";
import { useAppSelector, useAppDispatch } from "../store/configureStore";

export default function useCategories() {
    const categories = useAppSelector(categorySelectors.selectAll);
    const { categoriesLoaded, metaData} = useAppSelector(state => state.category);
    const dispatch = useAppDispatch();
  
    useEffect(() => {
      if (!categoriesLoaded) dispatch(fetchCategoriesAsync());
    }, [categoriesLoaded, dispatch])
  
    return {
        categories,
        categoriesLoaded,
        metaData
    }
}