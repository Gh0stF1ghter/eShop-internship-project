import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import brand from '../../../../models/brandModel';
import item from '../../../../models/itemModel';
import { BrandService } from '../../../../services/brand.service';

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html',
  styleUrl: './brand.component.css'
})
export class BrandComponent implements OnInit {
  brand: brand | undefined
  items: item[] = []

  constructor(
    private brandService: BrandService,
    private route: ActivatedRoute  
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    const brandId = Number(routeParams.get('brandId'));
    this.getBrand(brandId)
  }

  getBrand(brandId: number) {
    this.brandService.getBrandById(brandId).subscribe(brand => this.brand = brand);
  }
}