export class ItemSchema {
  id: string;
  name: string;
  properties: ItemSchemaProperty[];

  public constructor(init?: Partial<ItemSchema>) {
    Object.assign(this, init);
  }
}

export class ItemSchemaProperty {
  name: string;
  type: ItemSchemaPropertyType;
}

export enum ItemSchemaPropertyType {
  Text = 0,
  Number = 1
}
