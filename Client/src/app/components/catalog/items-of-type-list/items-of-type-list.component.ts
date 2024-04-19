import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import typeModel from '../../../models/typeModel';
import item from '../../../models/itemModel';
import pagination from '../../../models/pagination';
import { TypeService } from '../../../services/type.service';
import { ItemOfTypeService } from '../../../services/item-of-type.service';

@Component({
  selector: 'app-items-of-type-list',
  templateUrl: './items-of-type-list.component.html',
  styleUrl: './items-of-type-list.component.css',
})
export class ItemsOfTypeListComponent implements OnInit {
  type: typeModel | undefined;
  items: item[] = [];
  typeId: number | undefined;

  pagination: pagination | undefined;

  constructor(
    private typeService: TypeService,
    private itemService: ItemOfTypeService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.typeId = params['typeId'];

      console.log('subscribe');

      if (this.typeId) {
        this.getType(this.typeId);
        this.getItemsOfType(this.typeId);
      }
    });
  }

  getType(typeId: number) {
    this.typeService
      .getTypeById(typeId)
      .subscribe((type) => (this.type = type));
  }

  getItemsOfType(typeId: number) {
    this.itemService.getItemsOfType(typeId).subscribe((response) => {
      if (response.body) {
        this.items = response.body;

        let pageString = response.headers.get('Pagination');

        if (pageString) {
          this.pagination = JSON.parse(pageString);
        }
      }
    });
  }
}
