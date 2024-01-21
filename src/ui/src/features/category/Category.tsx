import { Edit, Delete } from '@mui/icons-material';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@mui/material';
import useCategories from '../../app/hooks/useCategories';
import AppPagination from '../../app/components/AppPagination';
import { useAppDispatch } from '../../app/store/configureStore';
import { removeCategory, setPageNumber } from '../category/categorySlice';
import { useState } from 'react';
import CategoryForm from './CategoryForm';
import { Category } from '../../app/models/category';
import agent from '../../app/api/agent';
import { LoadingButton } from '@mui/lab';

export default function Category() {
    const { categories, metaData } = useCategories();
    const [editMode, setEditMode] = useState(false);
    const dispatch = useAppDispatch();
    const [selectedCategory, setSelectedCategory] = useState<Category | undefined>(undefined);
    const [loading, setLoading] = useState(false);
    const [target, setTarget] = useState('');

    function handleSelectCategory(category: Category) {
        setSelectedCategory(category);
        setEditMode(true);
    }

    function cancelEdit() {
        if (selectedCategory) setSelectedCategory(undefined);
        setEditMode(false);
    }

    function handleDeleteCategory(id: string) {
        setLoading(true);
        setTarget(id)
        agent.Category.deleteCategory(id)
            .then(() => dispatch(removeCategory(id)))
            .catch(error => console.log(error))
            .finally(() => setLoading(false))
    }

    if (editMode) return <CategoryForm cancelEdit={cancelEdit} category={selectedCategory} />

    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2 }} variant='h4'>Category</Typography>
                <Button
                    sx={{ m: 2 }}
                    size='large' variant='contained'
                    onClick={() => setEditMode(true)}
                >
                    Create
                </Button>
            </Box>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>#</TableCell>
                            <TableCell align="left">Category</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {categories.map((category) => (
                            <TableRow
                                key={category.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    {category.id}
                                </TableCell>
                                <TableCell align="left">
                                    <Box display='flex' alignItems='center'>
                                        <span>{category.name}</span>
                                    </Box>
                                </TableCell>
                                <TableCell align="right">
                                    <Button
                                        startIcon={<Edit />}
                                        onClick={() => handleSelectCategory(category)}
                                    />
                                    <LoadingButton
                                        loading={loading && target === category.id}
                                        startIcon={<Delete />} color='error'
                                        onClick={() => handleDeleteCategory(category.id)}
                                    />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            {metaData &&
                <Box sx={{ pt: 2 }}>
                    <AppPagination
                        metaData={metaData}
                        onPageChange={(page: number) => dispatch(setPageNumber({ pageNumber: page }))}
                    />
                </Box>
            }
        </>
    )
}