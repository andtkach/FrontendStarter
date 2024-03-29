import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import { catalogSlice } from '../../features/catalog/catalogSlice';
import { accountSlice } from '../../features/account/accountSlice';
import { categorySlice } from '../../features/category/categorySlice';
import { personSlice } from '../../features/person/personSlice';

export const store = configureStore({
    reducer: {
        catalog: catalogSlice.reducer,
        account: accountSlice.reducer,
        category: categorySlice.reducer,
        person: personSlice.reducer,
    }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;