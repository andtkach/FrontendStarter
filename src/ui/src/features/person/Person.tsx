import { Edit, Delete } from '@mui/icons-material';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@mui/material';
import usePersons from '../../app/hooks/usePersons';
import AppPagination from '../../app/components/AppPagination';
import { useAppDispatch } from '../../app/store/configureStore';
import { removePerson, setPageNumber } from '../person/personSlice';
import { useState } from 'react';
import PersonForm from './PersonForm';
import { Person } from '../../app/models/person';
import agent from '../../app/api/agent';
import { LoadingButton } from '@mui/lab';

export default function Person() {
    const { persons, metaData } = usePersons();
    const [editMode, setEditMode] = useState(false);
    const dispatch = useAppDispatch();
    const [selectedPerson, setSelectedPerson] = useState<Person | undefined>(undefined);
    const [loading, setLoading] = useState(false);
    const [target, setTarget] = useState(0);

    function handleSelectPerson(person: Person) {
        setSelectedPerson(person);
        setEditMode(true);
    }

    function cancelEdit() {
        if (selectedPerson) setSelectedPerson(undefined);
        setEditMode(false);
    }

    function handleDeletePerson(id: number) {
        setLoading(true);
        setTarget(id)
        agent.Person.deletePerson(id)
            .then(() => dispatch(removePerson(id)))
            .catch(error => console.log(error))
            .finally(() => setLoading(false))
    }

    if (editMode) return <PersonForm cancelEdit={cancelEdit} person={selectedPerson} />

    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2 }} variant='h4'>Person</Typography>
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
                            <TableCell align="left">Person</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {persons.map((person) => (
                            <TableRow
                                key={person.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    {person.id}
                                </TableCell>
                                <TableCell align="left">
                                    <Box display='flex' alignItems='center'>
                                        <span>{person.name}</span>
                                    </Box>
                                </TableCell>
                                <TableCell align="right">
                                    <Button
                                        startIcon={<Edit />}
                                        onClick={() => handleSelectPerson(person)}
                                    />
                                    <LoadingButton
                                        loading={loading && target === person.id}
                                        startIcon={<Delete />} color='error'
                                        onClick={() => handleDeletePerson(person.id)}
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