import { Component, OnInit } from '@angular/core';
import { BrandService } from '../../../../services/brand.service';
import brand from '../../../../models/brandModel';

@Component({
  selector: 'app-admin-brands',
  templateUrl: './admin-brands.component.html',
  styleUrl: './admin-brands.component.css'
})
export class AdminBrandsComponent implements OnInit {
  brands: brand[] = []

  constructor(private brandService: BrandService) {}

  ngOnInit(): void {
    this.getBrands()
  }

  getBrands() {
    this.brandService.getAllBrands().subscribe(brands => this.brands = brands)
  }
}