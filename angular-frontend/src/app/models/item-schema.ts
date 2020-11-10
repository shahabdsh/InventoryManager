import { EntityBase } from "./entity-base";

export class ItemSchema extends EntityBase {
  id: string;
  name: string;
  properties: ItemSchemaProperty[];

  public constructor(init?: Partial<ItemSchema>) {
    super();
    Object.assign(this, init);
  }
}

export class ItemSchemaProperty {
  name: string;
  type: ItemSchemaPropertyType;
}

export enum ItemSchemaPropertyType {
  Text = 0,
  Number = 1,
  Checkbox = 2,
}
