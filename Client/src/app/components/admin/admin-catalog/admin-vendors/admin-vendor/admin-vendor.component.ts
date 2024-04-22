import { Component, OnInit } from '@angular/core';
import vendor from '../../../../../models/vendorModel';
import { VendorService } from '../../../../../services/vendor.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-vendor',
  templateUrl: './admin-vendor.component.html',
  styleUrl: './admin-vendor.component.css'
})
export class AdminVendorComponent implements OnInit {
  vendor: vendor | undefined;
  vendorId: number | undefined;

  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute
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
      .subscribe((vendor) => (this.vendor = vendor));
  }
}