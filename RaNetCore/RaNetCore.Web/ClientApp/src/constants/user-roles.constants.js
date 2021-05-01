export const userRoles = {
    Admin: { id: 'Admin', name: 'Admin' },
    SuperUser: { id: 'SuperUser', name: 'SuperUser' },
    BasicUser: { id: 'BasicUser', name: 'BasicUser' },
}

export const userRolesChoices = [
    userRoles.Admin,
    userRoles.SuperUser,
    userRoles.BasicUser,
]

const superUsersRoles = [userRoles.Admin.id, userRoles.SuperUser.id]
const allUserRoles = [...userRolesChoices.map((x) => x.id)]

export const userRolesGroups = {
    superUsers: superUsersRoles,
    everyone: allUserRoles,
}