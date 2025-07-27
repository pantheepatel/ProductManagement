import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ProductModel } from '../../core/models/product.model';
import { ProductService } from '../../services/product.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList {

  products!: ProductModel[];
  selectedProductId: string = '';

  @ViewChild('deleteModal') deleteModal: any;

  constructor(
    private productService: ProductService,
    private modalService: NgbModal,
    private router: Router,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.cd.detectChanges();
      },
      error: (error) => {
        this.toastr.error('Error fetching products', 'Error');
      }
    });
  }

  openDeleteModal(event: MouseEvent, id: string): void {
    event.stopPropagation(); // prevent row click
    this.selectedProductId = id;
    this.modalService.open(this.deleteModal, { centered: true });
  }

  confirmDelete(modalRef: any): void {
    console.log("delete product id:", this.selectedProductId);
    this.productService.deleteProduct(this.selectedProductId).subscribe({
      next: () => {
        this.products = this.products.filter(cat => cat.id !== this.selectedProductId);
        modalRef.close();
      },
      error: (error) => {
        this.toastr.error('Error deleting product', 'Error');
        modalRef.dismiss();
      },
      complete: () => {
        this.toastr.success('Product deleted successfully', 'Success');
        modalRef.close();
        this.loadProducts();
      }
    });
  }
}
