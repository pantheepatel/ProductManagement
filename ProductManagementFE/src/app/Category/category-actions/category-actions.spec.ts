import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryActions } from './category-actions';

describe('CategoryActions', () => {
  let component: CategoryActions;
  let fixture: ComponentFixture<CategoryActions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryActions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryActions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
