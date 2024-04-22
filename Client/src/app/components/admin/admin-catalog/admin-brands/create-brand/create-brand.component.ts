import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '../../../../../services/brand.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-brand',
  templateUrl: './create-brand.component.html',
  styleUrl: './create-brand.component.css'
})
export class CreateBrandComponent {
  form: FormGroup;

  constructor(
    private authService: BrandService,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      name: ['', Validators.required]
    });
  }

  async createBrand() {
    let brand = await this.authService.addBrand(this.form.value.name)

    if(brand) {
      this.router.navigate(['./'])
    }
  }
}
