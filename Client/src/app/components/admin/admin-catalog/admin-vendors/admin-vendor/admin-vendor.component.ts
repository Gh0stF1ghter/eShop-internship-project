import { Component, OnInit } from '@angular/core';
import vendor from '../../../../../models/vendorModel';
import { VendorService } from '../../../../../services/vendor.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin-vendor',
  templateUrl: './admin-vendor.component.html',
  styleUrl: './admin-vendor.component.css'
})
export class AdminVendorComponent implements OnInit {
  vendor: vendor | undefined;
  vendorId: number | undefined;

  inputOpened= false
  editName = '';

  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.vendorId = params['vendorId'];

      console.log('subscribe');

      if (this.vendorId) {
        this.getVendor(this.vendorId);
      }
    });
  }

  getVendor(vendorId: number) {
    this.vendorService
      .getVendorById(vendorId)
      .subscribe((vendor) => {
        this.vendor = vendor
        this.editName = vendor.name
      });
  }

  updateBrand() {
    if (this.vendorId) {
      if (this.editName !== this.vendor?.name) {
        this.vendorService
          .updateVendor(this.vendorId, this.editName)
          .subscribe((response) => {
            if (response.status === 204) {
              console.log('updated');
              this.router.navigate(['../', this.vendorId], {
                relativeTo: this.route,
              });
            }
          });
      }
    }
  }

  deleteBrand() {
    if (this.vendorId) {
      this.vendorService.deleteVendor(this.vendorId).subscribe((response) => {
        if (response.status === 204) {
          console.log('deleted');
          this.router.navigate(['../'], { relativeTo: this.route });
        }
      });
    }
  }
}