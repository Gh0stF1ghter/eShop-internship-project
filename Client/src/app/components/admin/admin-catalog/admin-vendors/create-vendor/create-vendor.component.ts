import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '../../../../../services/brand.service';
import { ActivatedRoute, Router } from '@angular/router';
import { VendorService } from '../../../../../services/vendor.service';

@Component({
  selector: 'app-create-vendor',
  templateUrl: './create-vendor.component.html',
  styleUrl: './create-vendor.component.css',
})
export class CreateVendorComponent {
  form: FormGroup;

  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      name: ['', Validators.required],
    });
  }

  async createBrand() {
    let brand = await this.vendorService.addVendor(this.form.value.name);

    if (brand) {
      this.router.navigate(['../', brand.id], { relativeTo: this.route });
    }
  }
}
