import { EntityBase } from "./entity-base";

export class Item extends EntityBase {

  id: string;
  name: string;
  schemaId: string;
  quantity: number;
  properties: ItemProperty[];

  public constructor(init?: Partial<Item>) {
    super();
    Object.assign(this, init);
  }
}

export class ItemProperty {
  key: string;
  value: string;
}
