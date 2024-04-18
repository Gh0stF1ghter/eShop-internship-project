import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import vendor from '../../../../models/vendorModel';
import { VendorService } from '../../../../services/vendor.service';

@Component({
  selector: 'app-vendor',
  templateUrl: './vendor.component.html',
  styleUrl: './vendor.component.css'
})
export class VendorComponent implements OnInit {
  vendor: vendor | undefined

  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute  
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    const vendorId = Number(routeParams.get('vendorId'));
    this.getBrand(vendorId)
  }

  getBrand(vendorId: number) {
    this.vendorService.getVendorById(vendorId).subscribe(vendor => this.vendor = vendor);
  }
}
