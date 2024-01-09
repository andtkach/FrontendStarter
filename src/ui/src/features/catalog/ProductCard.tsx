import { Button, Card, CardActions, CardContent, CardHeader } from "@mui/material";
import { Product } from "../../app/models/product";
import { Link } from 'react-router-dom';

interface Props {
    product: Product;
}

export default function ProductCard({ product }: Props) {
    
    return (
        <Card>
            <CardHeader
                title={product.name}
                titleTypographyProps={{
                    sx: { fontWeight: 'bold', color: 'primary.main' }
                }}
            />
            <CardContent>                
            </CardContent>
            <CardActions>
                <Button component={Link} to={`/catalog/${product.id}`} size="small">View</Button>
            </CardActions>
        </Card>
    )
}