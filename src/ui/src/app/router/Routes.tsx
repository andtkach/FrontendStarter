import { Navigate, createBrowserRouter } from 'react-router-dom';
import App from '../layout/App';
import Catalog from '../../features/catalog/Catalog';
import ProductDetails from '../../features/catalog/ProductDetails';
import AboutPage from '../../features/about/AboutPage';
import Login from '../../features/account/Login';
import Register from '../../features/account/Register';
import RequireAuth from './RequireAuth';
import Inventory from '../../features/admin/Inventory';
import Category from '../../features/category/Category';
import People from '../../features/person/Person';

export const router = createBrowserRouter(([
    {
        path: '/',
        element: <App />,
        children: [
            {
                // admin routes
                element: <RequireAuth />, children: [{ path: '/inventory', element: <Inventory /> }, ]
            },
            {
                element: <RequireAuth />, children: [{ path: '/category', element: <Category /> }, ]
            },
            {
                element: <RequireAuth />, children: [{ path: '/people', element: <People /> }, ]
            },
            { path: 'catalog', element: <Catalog /> },
            { path: 'catalog/:id', element: <ProductDetails /> },
            { path: 'about', element: <AboutPage /> },
            { path: '/login', element: <Login /> },
            { path: '/register', element: <Register /> },
            { path: '*', element: <Navigate replace to='/not-found' /> },
        ]
    }
]))