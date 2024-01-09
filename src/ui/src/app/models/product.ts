export interface Product {
    id: number,
    name: string,
    description: string,
}

export interface ProductParams {
    orderBy: string;
    searchTerm?: string;
    pageNumber: number;
    pageSize: number;
}