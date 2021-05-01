import GroupIcon from '@material-ui/icons/Group'
import { userRolesGroups } from './user-roles.constants'

const createResourceEntity = (menuLabel, basePath, icon, rolesWithAccess) => ({
    menuLabel: menuLabel,
    basePath: basePath,
    icon: icon,
    refId: `${basePath.slice(0, -1)}Id`, // removes the plural "s" in the end and concats "Id"
    rolesWithAccess,
})

const userEntity = createResourceEntity(
    'menu.users',
    'users',
    GroupIcon,
    userRolesGroups.management
)

export const resourcesMap= {
    users: userEntity
}