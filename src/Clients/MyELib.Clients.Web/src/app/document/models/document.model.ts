import { LibraryModel } from "../../library/models/library.model";

export interface DocumentModel {
  id: string;
  name: string;
  fileType: string;
  content: string; //base64-encoded string
  size: number;
  uploadedDate: Date;
  uploadedBy: string;
  library: LibraryModel;
}
