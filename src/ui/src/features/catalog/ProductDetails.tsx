import { Divider, Grid, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@mui/material';
import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import NotFound from '../../app/errors/NotFound';
import LoadingComponent from '../../app/layout/LoadingComponent';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { fetchProductAsync, productSelectors } from './catalogSlice';

export default function ProductDetails() {
    const { id } = useParams<{ id: string }>();
    const dispatch = useAppDispatch();
    const product = useAppSelector(state => productSelectors.selectById(state, id!));
    const {status: productStatus} = useAppSelector(state => state.catalog);
    
    useEffect(() => {
        if (!product && id) dispatch(fetchProductAsync(parseInt(id)))
    }, [id, product, dispatch]);

    if (productStatus.includes('pending')) return <LoadingComponent message='Loading product...' />

    if (!product) return <NotFound />

    return (
        <Grid container>
            <Grid item xs={12}>
                <Typography variant='h3'>{product.name}</Typography>
                <Divider sx={{ mb: 2 }} />
                <TableContainer>
                    <Table>
                        <TableBody sx={{ fontSize: '1.1em' }}>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>{product.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>{product.description}</TableCell>
                            </TableRow>                            
                        </TableBody>
                    </Table>
                </TableContainer>                
            </Grid>
        </Grid>
    )
}