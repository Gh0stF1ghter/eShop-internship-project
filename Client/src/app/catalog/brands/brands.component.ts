import { Component, OnInit } from '@angular/core';
import brand from '../../models/brandModel';
import { BrandService } from '../../services/brand.service';

@Component({
  selector: 'app-brands',
  templateUrl: './brands.component.html',
  styleUrl: './brands.component.css'
})
export class BrandsComponent implements OnInit {
  brands: brand[] = []

  constructor(private brandService: BrandService) {}

  ngOnInit(): void {
    this.getBrands()
  }

  getBrands() {
    this.brandService.getAllBrands().subscribe(brands => this.brands = brands)
  }
}
