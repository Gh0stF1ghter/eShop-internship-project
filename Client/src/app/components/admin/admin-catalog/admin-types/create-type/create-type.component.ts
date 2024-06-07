import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TypeService } from '../../../../../services/type.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-type',
  templateUrl: './create-type.component.html',
  styleUrl: './create-type.component.css'
})
export class CreateTypeComponent {
  form: FormGroup;

  constructor(
    private typeService: TypeService,
    private route: ActivatedRoute,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      name: ['', Validators.required]
    });
  }

  async createBrand() {
    let brand = await this.typeService.addType(this.form.value.name)

    if (brand) {
      this.router.navigate(['../', brand.id], { relativeTo: this.route });
    }
  }
}