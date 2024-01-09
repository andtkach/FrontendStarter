import { Box, Paper, Typography, Grid, Button } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import AppTextInput from '../../app/components/AppTextInput';
import { Product } from '../../app/models/product';
import { useEffect } from 'react';
import { yupResolver } from '@hookform/resolvers/yup';
import { validationSchema } from './productValidation';
import { useAppDispatch } from '../../app/store/configureStore';
import agent from '../../app/api/agent';
import { setProduct } from '../catalog/catalogSlice';
import { LoadingButton } from '@mui/lab';

interface Props {
    product?: Product;
    cancelEdit: () => void;
}

export default function ProductForm({ product, cancelEdit }: Props) {
    const { control, reset, handleSubmit, watch, formState: { isDirty, isSubmitting } } = useForm({
        mode: 'onTouched',
        resolver: yupResolver<any>(validationSchema)
    });
    const watchFile = watch('file', null);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (product && !watchFile && !isDirty) reset(product);
        return () => {
            if (watchFile) URL.revokeObjectURL(watchFile.preview);
        }
    }, [product, reset, watchFile, isDirty]);

    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Product;
            if (product) {
                response = await agent.Admin.updateProduct(data);
            } else {
                response = await agent.Admin.createProduct(data);
            }
            dispatch(setProduct(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Box component={Paper} sx={{ p: 4 }}>
            <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
                Product Details
            </Typography>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={12}>
                        <AppTextInput control={control} name='name' label='Product name' />
                    </Grid>
                    <Grid item xs={12}>
                        <AppTextInput
                            multiline={true}
                            rows={4}
                            control={control}
                            name='description'
                            label='Description'
                        />
                    </Grid>                    
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ mt: 3 }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit'>Cancel</Button>
                    <LoadingButton 
                        loading={isSubmitting}
                        type='submit' 
                        variant='contained' 
                        color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}