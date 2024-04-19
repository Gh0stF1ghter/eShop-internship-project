import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import vendor from '../../../../models/vendorModel';
import { VendorService } from '../../../../services/vendor.service';

@Component({
  selector: 'app-vendor',
  templateUrl: './vendor.component.html',
  styleUrl: './vendor.component.css',
})
export class VendorComponent implements OnInit {
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
