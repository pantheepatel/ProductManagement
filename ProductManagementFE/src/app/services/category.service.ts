import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { CategoryCreateDTO, CategoryDTO } from '../core/models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {

  private apiUrl = `${environment.baseUrl}api/Category`;

  constructor(private http: HttpClient) { }

  // GET: category list
  getCategories(): Observable<CategoryDTO[]> {
    return this.http.get<CategoryDTO[]>(`${this.apiUrl}`);
    // return of([
    //   { id: '1', name: 'Electronics', description: 'Devices and gadgets' },
    //   { id: '2', name: 'Books', description: 'Fiction and non-fiction books' },
    // ]);
  }

  // CREATE: add new category
  addCategory(category: CategoryCreateDTO): Observable<CategoryCreateDTO> {
    return this.http.post<CategoryDTO>(`${this.apiUrl}`, category);
    // return of(category); // Simulating a successful response
  }

  // GET: specific category
  getCategoryById(id: string): Observable<CategoryDTO> {
    return this.http.get<CategoryDTO>(`${this.apiUrl}/${id}`);
    // return of({
    //   id: '12',
    //   name: 'Sample Category',
    //   description: 'This is a sample category description.',
    // });
  }

  // UPDATE: edit existing category
  updateCategory(category: CategoryDTO): Observable<CategoryDTO> {
    return this.http.put<CategoryDTO>(`${this.apiUrl}/${category.id}`, category);
    // return of(category); // Simulating a successful response
  }

  // DELETE: remove category
  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
    // return of(); // Simulating a successful response
  }
}