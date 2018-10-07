import { Guid } from './guid';

export class Uploader {
  id: string;
  file: File;

  constructor(file: File) {
    this.file = file;
    this.id = Guid.newGuid();
  }
}  