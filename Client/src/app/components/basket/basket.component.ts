import { Component, OnInit } from '@angular/core';
import basketItem from '../../models/basket/basketItemModel';
import { SignalrService } from '../../services/signalr.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css',
})
export class BasketComponent implements OnInit {
  userId: string | undefined;

  constructor(
    public signalrService: SignalrService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.userId = params['userId'];

      if (this.userId) {
        this.signalrService.startConnection(this.userId);
        this.signalrService.addUserBasketListener();
        this.signalrService.addBasketItemsListener();
      }
    });
  }

  async updateQuantity(basketItem: basketItem, quantity: number) {
    if (this.userId) {
      if (quantity <= 0) {
        quantity = 1;
      }

      await this.signalrService.updateBasketItemQuantity(
        basketItem,
        this.userId,
        quantity
      );
    }
  }

  async deleteItem(basketItemId: string) {
    if (this.userId) {
      await this.signalrService.deleteBasketItem(this.userId, basketItemId)
    }
  }
}
