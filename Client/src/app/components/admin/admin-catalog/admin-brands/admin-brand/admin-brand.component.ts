import { Component, OnInit } from '@angular/core';
import brand from '../../../../../models/brandModel';
import item from '../../../../../models/itemModel';
import { BrandService } from '../../../../../services/brand.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-brand',
  templateUrl: './admin-brand.component.html',
  styleUrl: './admin-brand.component.css'
})
export class AdminBrandComponent implements OnInit {
  brand: brand | undefined;
  items: item[] = [];
  brandId: number | undefined;

  constructor(
    private brandService: BrandService,
    private route: ActivatedRoute
  ) {}


  ngOnInit(): void {
    const routeParams = this.route.params;

    //Do the same to the others
    routeParams.subscribe((params) => {
      this.brandId = params['brandId'];

      console.log('subscribe');
      
      if (this.brandId) {
        this.getBrand(this.brandId);
      }
    });
  }

  //Replace to other lifecycle hook

  getBrand(brandId: number) {
    this.brandService
      .getBrandById(brandId)
      .subscribe((brand) => (this.brand = brand));
  }
}
