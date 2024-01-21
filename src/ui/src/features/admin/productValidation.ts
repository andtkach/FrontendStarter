import * as yup from 'yup';

export const validationSchema = yup.object({
    name: yup.string().required(),
    description: yup.string().required(),    
})