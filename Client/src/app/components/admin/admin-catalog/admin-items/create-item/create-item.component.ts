import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ItemOfTypeService } from '../../../../../services/item-of-type.service';
import { ActivatedRoute, Router } from '@angular/router';
import item from '../../../../../models/itemModel';
import createItem from '../../../../../models/createItemModel';

@Component({
  selector: 'app-create-item',
  templateUrl: './create-item.component.html',
  styleUrl: './create-item.component.css',
})
export class CreateItemComponent {
  form: FormGroup;

  constructor(
    private vendorService: ItemOfTypeService,
    private route: ActivatedRoute,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      name: ['', Validators.required],
      typeId: [0, Validators.nullValidator],
      brandId: [0, Validators.nullValidator],
      vendorId: [0, Validators.nullValidator],
      stock: [0, Validators.nullValidator],
      price: [0, Validators.nullValidator],
    });
  }

  async createBrand() {
    let itemToCreate: createItem = {
      name: this.form.value.name,
      brandId:this.form.value.brandId,
      vendorId:this.form.value.vendorId,
      stock:this.form.value.stock,
      price:this.form.value.price,
    };

    let brand = await this.vendorService.addItemOfType(this.form.value.typeId, itemToCreate);

    if (brand) {
      this.router.navigate(['../', brand.id], { relativeTo: this.route });
    }
  }
}
