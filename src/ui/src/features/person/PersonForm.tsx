import { Box, Paper, Typography, Grid, Button } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import AppTextInput from '../../app/components/AppTextInput';
import { Person } from '../../app/models/person';
import { useEffect } from 'react';
import { yupResolver } from '@hookform/resolvers/yup';
import { validationSchema } from './personValidation';
import { useAppDispatch } from '../../app/store/configureStore';
import agent from '../../app/api/agent';
import { setPerson } from '../person/personSlice';
import { LoadingButton } from '@mui/lab';

interface Props {
    person?: Person;
    cancelEdit: () => void;
}

export default function PersonForm({ person, cancelEdit }: Props) {
    const { control, reset, handleSubmit, watch, formState: { isDirty, isSubmitting } } = useForm({
        mode: 'onTouched',
        resolver: yupResolver<any>(validationSchema)
    });
    const watchFile = watch('file', null);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (person && !watchFile && !isDirty) reset(person);
        return () => {
            if (watchFile) URL.revokeObjectURL(watchFile.preview);
        }
    }, [person, reset, watchFile, isDirty]);

    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Person;
            if (person) {
                response = await agent.Person.updatePerson(data);
            } else {
                response = await agent.Person.createPerson(data);
            }
            dispatch(setPerson(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Box component={Paper} sx={{ p: 4 }}>
            <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
                Person Details
            </Typography>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={12}>
                        <AppTextInput control={control} name='name' label='Person name' />
                    </Grid>
                    <Grid item xs={12} sm={12}>
                        <AppTextInput control={control} name='email' type='email' label='Person email' />
                    </Grid>
                    <Grid item xs={12}>
                        <AppTextInput
                            multiline={true}
                            rows={4}
                            control={control}
                            name='address'
                            label='Address'
                        />
                    </Grid>
                    <Grid item xs={12} sm={12}>
                        <AppTextInput control={control} name='age' type='number' label='Person age' />
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