import {Component, inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    NgIf,
    NgForOf
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  httpClient: HttpClient = inject(HttpClient);
  data: string = '';

  cartItems: string[] = [];

  ngOnInit(): void {
    this.getCartItems();
  }

  getCartItems(): void {
    this.httpClient.get<any>('/api/cart').subscribe((data: any) => {
      this.cartItems = data.result;
    });
  }

  removeFromCart(seatId: string): void {
    this.httpClient.post(`/api/cart/remove/${seatId}`, {}).subscribe(() => {
      this.getCartItems();
    });
  }
}
