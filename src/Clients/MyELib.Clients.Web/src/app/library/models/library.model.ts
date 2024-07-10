import { DocumentModel } from "../../document/models/document.model"
import { LibraryUserModel } from "../../library-user/models/library-user.model"

export interface LibraryModel {
  id: string;
  name: string;
  documents: DocumentModel[];
  libraryUsers: LibraryUserModel[];
}
