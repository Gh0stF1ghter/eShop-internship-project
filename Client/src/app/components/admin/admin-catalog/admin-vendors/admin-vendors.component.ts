import { Component, OnInit } from '@angular/core';
import vendor from '../../../../models/vendorModel';
import { VendorService } from '../../../../services/vendor.service';

@Component({
  selector: 'app-admin-vendors',
  templateUrl: './admin-vendors.component.html',
  styleUrl: './admin-vendors.component.css'
})
export class AdminVendorsComponent  implements OnInit {
  vendors: vendor[] = []

  constructor(private vendorService: VendorService) {}

  ngOnInit(): void {
    this.getVendors()
  }

  getVendors() {
    this.vendorService.getAllVendors().subscribe(vendors => this.vendors = vendors)
  }
}
