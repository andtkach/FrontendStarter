import { Alert, AlertTitle, Button, ButtonGroup, Container, List, ListItem, ListItemText, Typography } from '@mui/material';
import { useState } from 'react';
import agent from '../../app/api/agent';

export default function AboutPage() {
    const [info, setInfo] = useState<any>(null);

    function getBffInfo() {
        agent.Info.getInfo()
            .then((data) => {
                console.log(data); setInfo(data);
            });
    }

    return (
        <Container>
            <Typography gutterBottom variant={'h4'}>Frontend starter project</Typography>
            <ButtonGroup fullWidth>
                <Button onClick={getBffInfo} variant={'contained'}>BFF Info</Button>
                
            </ButtonGroup>
            {info &&
                <Alert severity="info">
                    <AlertTitle>Info</AlertTitle>
                    <List>
                            <ListItem key={'app'}>
                                <ListItemText>App: {info.bff}</ListItemText>
                            </ListItem>
                            <ListItem key={'ver'}>
                                <ListItemText>Ver: {info.version}</ListItemText>
                            </ListItem>
                            <ListItem key={'url'}>
                                <ListItemText>Url: {import.meta.env.VITE_API_URL}</ListItemText>
                            </ListItem>
                    </List>
                </Alert>}
        </Container>
    )
}