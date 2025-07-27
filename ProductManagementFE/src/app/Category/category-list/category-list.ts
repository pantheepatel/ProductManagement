import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryDTO } from '../../core/models/category.model';
import { CategoryService } from '../../services/category.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList {
  categories!: CategoryDTO[];
  selectedCategoryId: string = '';

  @ViewChild('deleteModal') deleteModal: any;

  constructor(
    private categoryService: CategoryService,
    private modalService: NgbModal,
    private router: Router,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef // ChangeDetectorRef to handle form state changes
  ) { }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.cd.detectChanges();
      },
      error: (error) => {
        this.toastr.error('Error fetching categories', 'Error');
      }
    });
  }

  openDeleteModal(event: MouseEvent, id: string): void {
    event.stopPropagation(); // prevent row click
    this.selectedCategoryId = id;
    this.modalService.open(this.deleteModal, { centered: true });
  }

  confirmDelete(modalRef: any): void {
    console.log("delete category id:", this.selectedCategoryId);
    this.categoryService.deleteCategory(this.selectedCategoryId).subscribe({
      next: () => {
        this.categories = this.categories.filter(cat => cat.id !== this.selectedCategoryId);
        modalRef.close();
      },
      error: (error) => {
        this.toastr.error('Error deleting category', 'Error');
        modalRef.dismiss();
      },
      complete: () => {
        this.toastr.success('Category deleted successfully', 'Success');
        modalRef.close();
        this.getCategories();
      }
    });
  }
}
