import { Box, Typography } from "@mui/material";

export default function HomePage() {

    return (
        <>
            <Box display='flex' justifyContent='center' sx={{ p: 4 }} >
                <Typography variant='h1'>
                    Welcome to the store!
                </Typography>
            </Box>
        </>
    )
}