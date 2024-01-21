import * as yup from 'yup';

export const validationSchema = yup.object({
    name: yup.string().required(),
    email: yup.string().required(),
    address: yup.string().required(),
    age: yup.number().required(),
})