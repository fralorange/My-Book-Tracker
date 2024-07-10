import { AuthRoles } from "../../roles/models/roles.enum"

export interface LibraryUserModel {
  id: string;
  libraryId: string;
  userId: string;
  role: AuthRoles;
}
