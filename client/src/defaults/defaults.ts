import { InjectionToken } from '@angular/core';

import { GroupBy } from '../app/shared/enums/group-by.enum';
import { OrderBy } from '../app/shared/enums/order-by.enum';

export let DEFAULTS = new InjectionToken('defaults');

export interface IDefaults {
	antiforgeryHeaderName: string;
	antiforgeryCookieName: string;
	path: string;
	groupBy: GroupBy;
	orderBy: OrderBy;
}

export const Defaults: IDefaults = {
	antiforgeryHeaderName: 'X-Zeus-Lightning',
	antiforgeryCookieName: 'Zeus-Lightning',
	path: '/',
	groupBy: GroupBy.FileSystemInfoType,
	orderBy: OrderBy.None
};
