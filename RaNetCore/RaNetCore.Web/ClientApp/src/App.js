import React from 'react';
import { Resource } from 'react-admin'
import { AuthAdmin } from 'ra-auth-ui'
import { createBrowserHistory as createHistory } from 'history'
import { authProvider, dataProvider } from './ra-providers';
import { crudResources } from './components';

const history = createHistory()

export const App = (props) => {
	return (
		<AuthAdmin
			{...props}
			title="Ra.NetCore"
			authLayout={{ userMenu: true, menu: undefined }}
			authProvider={authProvider}
			dataProvider={dataProvider}
			history={history}
			// theme={theme}
			//profilePage={true}
			//dashboard={Dashboard}
			// i18nProvider={i18nProvider}
			// customRoutes={routesProvider}
		>
			{addCrudResources(crudResources, null)}
		</AuthAdmin>
	);
}

const addCrudResources = (resCollection, permissions) => {
	return resCollection.map(
		(x, i) =>
		//hasAccessToResource(permissions, x.rolesWithAccess) &&
		(
			<Resource key={i} name={x.basePath} {...x.crud} />
		)
	)
}
