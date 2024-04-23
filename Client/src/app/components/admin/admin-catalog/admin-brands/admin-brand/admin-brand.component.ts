import { Component, OnInit } from '@angular/core';
import brand from '../../../../../models/brandModel';
import item from '../../../../../models/itemModel';
import { BrandService } from '../../../../../services/brand.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin-brand',
  templateUrl: './admin-brand.component.html',
  styleUrl: './admin-brand.component.css',
})
export class AdminBrandComponent implements OnInit {
  brand: brand | undefined;
  items: item[] = [];
  brandId: number | undefined;

  inputOpened= false
  editName = '';

  constructor(
    private brandService: BrandService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const routeParams = this.route.params;

    //Do the same to the others
    routeParams.subscribe((params) => {
      this.brandId = params['brandId'];

      console.log('subscribe');

      if (this.brandId) {
        this.getBrand(this.brandId);
        this.inputOpened=false
      }
    });
  }

  //Replace to other lifecycle hook

  getBrand(brandId: number) {
    this.brandService.getBrandById(brandId).subscribe((brand) => {
      this.brand = brand;
      this.editName = brand.name;
    });
  }

  updateBrand() {
    if (this.brandId) {
      if (this.editName !== this.brand?.name) {
        this.brandService
          .updateBrand(this.brandId, this.editName)
          .subscribe((response) => {
            if (response.status === 204) {
              console.log('updated');
              this.router.navigate(['../', this.brandId], {
                relativeTo: this.route,
              });
            }
          });
      }
    }
  }

  deleteBrand() {
    if (this.brandId) {
      this.brandService.deleteBrand(this.brandId).subscribe((response) => {
        if (response.status === 204) {
          console.log('deleted');
          this.router.navigate(['../'], { relativeTo: this.route });
        }
      });
    }
  }
}
