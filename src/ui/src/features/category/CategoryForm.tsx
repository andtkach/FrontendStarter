import { Box, Paper, Typography, Grid, Button } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import AppTextInput from '../../app/components/AppTextInput';
import { Category } from '../../app/models/category';
import { useEffect } from 'react';
import { yupResolver } from '@hookform/resolvers/yup';
import { validationSchema } from './categoryValidation';
import { useAppDispatch } from '../../app/store/configureStore';
import agent from '../../app/api/agent';
import { setCategory } from '../category/categorySlice';
import { LoadingButton } from '@mui/lab';

interface Props {
    category?: Category;
    cancelEdit: () => void;
}

export default function CategoryForm({ category, cancelEdit }: Props) {
    const { control, reset, handleSubmit, watch, formState: { isDirty, isSubmitting } } = useForm({
        mode: 'onTouched',
        resolver: yupResolver<any>(validationSchema)
    });
    const watchFile = watch('file', null);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (category && !watchFile && !isDirty) reset(category);
        return () => {
            if (watchFile) URL.revokeObjectURL(watchFile.preview);
        }
    }, [category, reset, watchFile, isDirty]);

    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Category;
            if (category) {
                response = await agent.Category.updateCategory(data);
            } else {
                response = await agent.Category.createCategory(data);
            }
            dispatch(setCategory(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Box component={Paper} sx={{ p: 4 }}>
            <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
                Category Details
            </Typography>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={12}>
                        <AppTextInput control={control} name='name' label='Category name' />
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