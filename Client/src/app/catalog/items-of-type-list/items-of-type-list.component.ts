import { Component, OnInit } from '@angular/core';
import { TypeService } from '../../services/type.service';
import typeModel from '../../models/typeModel';
import item from '../../models/itemModel';
import { ActivatedRoute } from '@angular/router';
import { ItemOfTypeService } from '../../services/item-of-type.service';
import pagination from '../../models/pagination';

@Component({
  selector: 'app-items-of-type-list',
  templateUrl: './items-of-type-list.component.html',
  styleUrl: './items-of-type-list.component.css',
})
export class ItemsOfTypeListComponent implements OnInit {
  type: typeModel | undefined
  items: item[] = []

  pagination: pagination | undefined

  constructor(
    private typeService: TypeService,
    private itemService: ItemOfTypeService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap
    const typeId = Number(routeParams.get('typeId'))

    this.getType(typeId)
    this.getItemsOfType(typeId)
  }

  getType(typeId: number) {
    this.typeService
      .getTypeById(typeId)
      .subscribe((type) => (this.type = type))
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
    })
  }
}
