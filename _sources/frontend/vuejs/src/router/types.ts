import { RouteRecordRaw } from 'vue-router';

// Définition du type CustomRouteRecordRaw
export type CustomRouteRecordRaw = RouteRecordRaw & {
  icon?: string;
};
