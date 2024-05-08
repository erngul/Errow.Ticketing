import { Routes } from '@angular/router';
import {CreateeventplacementsComponent} from "./create-eventplacement/create-eventplacements.component";
import {eventplacementComponent} from "./eventplacement/eventplacement.component";
import {CartComponent} from "./cart/cart.component";

export const routes: Routes = [
  { path: '', component: CreateeventplacementsComponent },
  { path: 'seats', component: eventplacementComponent },
  { path: 'cart', component: CartComponent },
];
