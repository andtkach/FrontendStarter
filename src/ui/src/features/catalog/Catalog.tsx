import ProductList from './ProductList';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { setPageNumber, setProductParams } from './catalogSlice';
import { Grid, Paper } from '@mui/material';
import ProductSearch from './ProductSearch';
import RadioButtonGroup from '../../app/components/RadioButtonGroup';
import AppPagination from '../../app/components/AppPagination';
import useProducts from '../../app/hooks/useProducts';

const sortOptions = [
    { value: 'nameAZ', label: 'A-Z' },
    { value: 'nameZA', label: 'Z-A' },
]

export default function Catalog() {
    const {products, metaData} = useProducts();
    const { productParams } = useAppSelector(state => state.catalog);
    const dispatch = useAppDispatch();

    return (
        <Grid container columnSpacing={4}>
            <Grid item xs={3}>
                <Paper sx={{ mb: 2 }}>
                    <ProductSearch />
                </Paper>
                <Paper sx={{ p: 2, mb: 2 }}>
                    <RadioButtonGroup
                        selectedValue={productParams.orderBy}
                        options={sortOptions}
                        onChange={(e) => dispatch(setProductParams({ orderBy: e.target.value }))}
                    />
                </Paper>                
            </Grid>
            <Grid item xs={9}>
                <ProductList products={products} />
            </Grid>
            <Grid item xs={3} />
            <Grid item xs={9} sx={{mb:2, mt:2}}>
                {metaData &&
                <AppPagination 
                    metaData={metaData}
                    onPageChange={(page: number) => dispatch(setPageNumber({pageNumber: page}))}
                />}
            </Grid>
        </Grid>
    )
}