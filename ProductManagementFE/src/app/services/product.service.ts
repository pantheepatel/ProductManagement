import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ProductCreateDTO, ProductModel } from "../core/models/product.model";
import { Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ProductService {

    private apiUrl = `${environment.baseUrl}api/Product`;

    constructor(private http: HttpClient) { }

    // GET: product list
    getProducts(): Observable<ProductModel[]> {
        return this.http.get<ProductModel[]>(`${this.apiUrl}`);
        // return of([
        //     { id: '1', name: 'Laptop', description: 'High performance laptop', basePrice: 1000, categoryId: '1', tax: 0.2, currentPrice: 1200, prices: [] },
        //     { id: '2', name: 'Smartphonex', description: 'Latest model smartphone', basePrice: 800, categoryId: '1', tax: 0.15, currentPrice: 600, prices: [] },
        // ]);
    }

    // CREATE: add new product
    addProduct(product: ProductCreateDTO): Observable<ProductCreateDTO> {
        return this.http.post<ProductCreateDTO>(`${this.apiUrl}`, product);
        // return of(product); // Simulating a successful response
    }

    // GET: specific product
    getProductById(id: string): Observable<ProductModel> {
        return this.http.get<ProductModel>(`${this.apiUrl}/${id}`);
        // return of({
        //     id: '1',
        //     name: 'Laptop',
        //     description: 'High performance laptop',
        //     basePrice: 1000,
        //     categoryId: '1',
        //     tax: 0.2,
        //     currentPrice: 1200,
        //     prices: [
        //         {
        //             seasonalPrice: 950,
        //             startDate: '2025-08-01',
        //             endDate: '2025-08-15'
        //         },
        //         {
        //             seasonalPrice: 980,
        //             startDate: '2025-12-01',
        //             endDate: '2025-12-31'
        //         }
        //     ],
        // });
    }

    // UPDATE: edit specific product
    updateProduct(product: ProductModel): Observable<ProductModel> {
        return this.http.put<ProductModel>(`${this.apiUrl}/${product.id}`, product);
        // return of(product); // Simulating a successful response
    }

    // DELETE: remove product
    deleteProduct(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
        // return of(); // Simulating a successful response
    }
}